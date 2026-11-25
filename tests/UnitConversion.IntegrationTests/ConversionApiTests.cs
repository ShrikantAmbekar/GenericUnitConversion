using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UnitConversion.Application.DTOs;

namespace UnitConversion.IntegrationTests;

[TestFixture]
public sealed class ConversionApiTests
{
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task PostConversion_ValidRequest_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/conversions", new
        {
            value = 10m,
            fromUnit = "meter",
            toUnit = "feet"
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.That(body, Is.Not.Null);
        Assert.That(body!.ConvertedValue, Is.EqualTo(32.8084m).Within(0.0001m));
    }

    [Test]
    public async Task PostConversion_CrossCategory_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/conversions", new
        {
            value = 10m,
            fromUnit = "meter",
            toUnit = "kilogram"
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.That(json.GetProperty("message").GetString(), Does.Contain("categories"));
    }

    [Test]
    public async Task PostConversion_UnknownUnit_ReturnsNotFound()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/conversions", new
        {
            value = 1m,
            fromUnit = "parsec",
            toUnit = "meter"
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetUnits_ReturnsCatalog()
    {
        var response = await _client.GetAsync("/api/v1/units");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var units = await response.Content.ReadFromJsonAsync<List<UnitResponse>>();
        Assert.That(units, Is.Not.Null);
        Assert.That(units!.Count, Is.GreaterThanOrEqualTo(15));
    }

    [Test]
    public async Task PostConversion_CentimeterToMeter_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/conversions", new
        {
            value = 100m,
            fromUnit = "centimeter",
            toUnit = "meter"
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var body = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.That(body!.ConvertedValue, Is.EqualTo(1m));
    }

    [Test]
    public async Task PostConversion_MilliliterToLiter_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/conversions", new
        {
            value = 2500m,
            fromUnit = "milliliter",
            toUnit = "liter"
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var body = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.That(body!.ConvertedValue, Is.EqualTo(2.5m));
        Assert.That(body.Category, Is.EqualTo("Volume"));
    }

    [Test]
    public async Task GetUnitsByCategory_ReturnsFilteredList()
    {
        var response = await _client.GetAsync("/api/v1/units/Temperature");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var units = await response.Content.ReadFromJsonAsync<List<UnitResponse>>();
        Assert.That(units, Has.Count.EqualTo(3));
    }

    [Test]
    public async Task GetUnitsByCategory_Volume_ReturnsFilteredList()
    {
        var response = await _client.GetAsync("/api/v1/units/Volume");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var units = await response.Content.ReadFromJsonAsync<List<UnitResponse>>();
        Assert.That(units, Has.Count.EqualTo(2));
    }
}