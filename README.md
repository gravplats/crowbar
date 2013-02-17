Crowbar - because sometimes you need leverage
=============================================

Crowbar is an application testing library for ASP.NET MVC 3 and 4.

Credits
-------

Crowbar was inspired by the Nancy.Testing project from [Nancy](https://github.com/NancyFx/Nancy) by Andreas Håkansson, Steven Robbins and contributors. The Crowbar API is highly influenced by the Nancy.Testing API.

The source code of Crowbar is based on Steven Sanderson's [MvcIntegrationTestFramework](http://blog.stevensanderson.com/2009/06/11/integration-testing-your-aspnet-mvc-application/). The initial commit of Crowbar is Jon Canning's [fork](https://github.com/JonCanning/MvcIntegrationTestFramework) of the MvcIntegrationTestFramework.

Crowbar employs [James Treworgy's](http://blog.outsharked.com) jQuery port [CsQuery](https://github.com/jamietre/CsQuery) for CSS selection and DOM manipulation. CsQuery is the only third-party dependency of Crowbar.

Table of Contents
-----------------
* [MvcApplication](#section-mvcapplication)
    * [User-Defined Context](#section-mvcapplication-context)
    * [User-Defined Proxy](#section-mvcapplication-proxy)
    * [User-Supplied Web.config](#section-mvcapplication-config)
    * [Default BrowserContext](#section-default-browsercontext)
* [Browser](#section-browser)
    * [Extensions](#section-browser-extensions)
* [BrowserContext](#section-browsercontext)
    * [AjaxRequest](#section-browsercontext-ajaxrequest)
    * [Body](#section-browsercontext-body)
    * [Cookie](#section-browsercontext-cookie)
    * [FormValue](#section-browsercontext-formvalue)
    * [Header](#section-browsercontext-header)
    * [HttpRequest](#section-browsercontext-httprequest)
    * [HttpsRequest](#section-browsercontext-httpsrequest)
    * [Query](#section-browsercontext-query)
    * [Extensions](#section-browsercontext-extensions)
* [BrowserResponse](#section-browserresponse)
    * [Advanced](#section-browserresponse-advanced)
    * [ContentType](#section-browserresponse-contenttype)
    * [Headers](#section-browserresponse-headers)
    * [ResponseBody](#section-browserresponse-responsebody)
    * [StatusCode](#section-browserresponse-statuscode)
    * [AsCsQuery](#section-browserresponse-ascsquery)
    * [AsJson](#section-browserresponse-asjson)
    * [AsXml](#section-browserresponse-asxml)
    * [Extensions](#section-browserresponse-extensions)
* [Samples](#section-samples)
* [Troubleshooting](#section-troubleshooting)
* [Changelog](#section-changelog)

<a name="section-mvcapplication"></a>
MvcApplication
--------------

The `MvcApplication` class (both generic and non-generic) is the heart of Crowbar and represents a proxy to the ASP.NET MVC project. Please note that creating the `MvcApplication` instance is a time-consuming process and should preferably only be done once, e.g., in a test base class.

An instance of `MvcApplication` can be created in two ways.

``` csharp
public class MvcApplicationTests
{
    private static readonly MvcApplication Application =
        MvcApplication.Create("<name of your ASP.NET MVC project>");

    [Test]
    public void Should_return_html()
    {
        Application.Execute(browser =>
        {
            var response = browser.Get("/route");
            response.ShouldBeHtml();
        });        
    }
}
```

``` csharp
public class MvcApplicationTests
{
    protected static readonly MvcApplication<UserDefinedContext> Application = 
        MvcApplication.Create<UserDefinedProxy, UserDefinedContext>("<name of your ASP.NET MVC project>");

    [Test]
    public void Should_return_html()
    {
        Application.Execute((browser, context) =>
        {
            // Use any state in the context to bootstrap your test.

            var response = browser.Get("/route");
            response.ShouldBeHtml();
        });
    }
}
```

<a name="section-mvcapplication-context"></a>
### User-Defined Context

In order to be able to pass state from the server (the MVC application) to the test case it is possible to define a user-defined context. The context must implement `IDisposable`. The context will be created for each test and will be disposed after the test has been run.

``` csharp
public class UserDefinedContext : IDisposable
{
    // Define any state from the server.

    public void Dispose()
    {
        // Will be disposed after the test case has been run.
    }
}
```

<a name="section-mvcapplication-proxy"></a>
### User-Defined Proxy

In order to initialize the user-defined context we need to defined our own proxy. This is done by deriving from `MvcApplicationProxyBase<TContext>`.

``` csharp
public class UserDefinedProxy : MvcApplicationProxyBase<UserDefinedContext>
{
    protected override UserDefinedContext CreateContext(HttpApplication application, string testBaseDirectory)
    {
        // You probably want to cast the 'application' to your derived HttpApplication class.

        return new UserDefinedContext
        {
            // Set any state.
        };
    }
}
```

<a name="section-mvcapplication-config"></a>
### User-Supplied Web.config

By default Crowbar tries to use the Web.config defined in the ASP.NET MVC project, even though this is somewhat instable at times. To counter any problems with the configuration of the MVC project it is possible and highly recommended to use a custom configuration file. The name of the custom configuration file can be supplied as the second argument to `MvcApplication.Create()`. The custom configration file should be defined in the test project. Please note that the configuration file must be copied to the output directory by setting _Copy to Output Directory_ to either _Copy always_ or _Copy if newer_.

``` csharp
private static readonly MvcApplication Application =
    MvcApplication.Create("<name of your ASP.NET MVC project>", "Web.Custom.config");
```

<a name="section-default-browsercontext"></a>
### Default BrowserContext

It is possible to define default `BrowserContext` settings for each request by supplying a third argument to `MvcApplication.Create()`. These settings will be applied to the `BrowserContext` before the request-specific settings. See the section on [BrowserContext](#section-browsercontext) for available options.

``` csharp
private static readonly MvcApplication Application =
    MvcApplication.Create("<name of your ASP.NET MVC project>", "Web.Custom.config", ctx => ctx.HttpsRequest());
```

<a name="section-browser"></a>
Browser
-------

An instance of the `Browser` class enables us to interact with ASP.NET MVC application by performing requests.

``` csharp
var response = browser.PerformRequest("GET", "/route");
```

The `PerformRequest` method has an optional third argument which let us customize the request.

``` csharp
var response = browser.PerformRequest("GET", "/route", ctx => {
    // Customize the request (BrowserContext object).
});
```

For convenience, the most common HTTP methods have their own methods: `Delete`, `Get`, `Post`, `Put`, which delegate to `PerformRequest`. `PerformRequest` returns an instance of `BrowserResponse`.

<a name="section-browser-extensions"></a>
### Submit / AjaxSubmit

The `Submit` and `AjaxSubmit` extensions try to mimic the browser form submission behavior. This is done by extracting the form action, the form method, any form values for the supplied HTML and performing the request based on these values.

``` csharp
var response = browser.Submit("<form action='/route' method='post'>...</form>", model);
```

### Load
Loads HTML from the server using GET and forwards it to either `Submit` or `AjaxSubmit`.

```csharp
var continuation = browser.Load("/path/to/form", ctx => { // Modify the GET request. });
var response = continuation.Submit(model);
```

### Render
Renders HTML by reading it from disk and forwards it to either `Submit` or `AjaxSubmit`. The first argument can be either a string (the name of the view) or a `PartialViewContext` object. Using the partial view context it is possible to state whether client validation and/or unobtrusive JavaScript is enabled and to set the principal that should be used when rendering the view which is important when the form is rendered with an anti-forgery request token.

``` csharp
var continuation = browser.Render("~/path/to/form.cshtml", model);
var response = continuation.Submit();
```

``` csharp
var context = new PartialViewContext("~/path/to/form.cshtml")
{
    ClientValidationEnabled = true,
    UnobtrusiveJavaScriptEnabled = true
};

context.SetFormsAuthPrincipal("username");

var response = browser.Render(context, model).Submit();
```

<a name="section-browsercontext"></a>
Browser Context
---------------

The `BrowserContext` class defines the context of the HTTP request. As previously stated `Browser.PerformRequest()` provides us with the opportunity to customize the HTTP request.

<a name="section-browsercontext-ajaxrequest"></a>
### AjaxRequest

To mark the request as an AJAX request call `AjaxRequest()`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    ctx.AjaxRequest();
});
```

<a name="section-browsercontext-body"></a>
### Body

To set the body of the request use `Body(string body, string contentType`). The second parameter is optional (the default content type is _application/octet-stream_).

<a name="section-browsercontext-cookie"></a>
### Cookie

To supply a cookie with the request use `Cookie(HttpRequest cookie)`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    HttpCookie = ...
    ctx.Cookie(cookie);
});
```

<a name="section-browsercontext-formvalue"></a>
### FormValue

To provide a form value with the request use `FormValue(string key, string value)`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    ctx.FromValue("key", "value");
});
```

<a name="section-browsercontext-header"></a>
### Header

To provide an HTTP header with the request use `Header(string name, string value)`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    ctx.Header("name", "value");
});
```

<a name="section-browsercontext-httprequest"></a>
### HttpRequest

To mark the request as using the HTTP protocol (the default value) use `HttpRequest()`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    ctx.HttpRequest();
});
```

<a name="section-browsercontext-httpsrequest"></a>
### HttpsRequest

To mark the request as using the HTTPS protocol use `HttpsRequest()`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    ctx.HttpsRequest();
});
```

<a name="section-browsercontext-query"></a>
### Query

To provide a query string entry with the request use `Query(string key, string value)`.

``` csharp
var response = browser.PerformRequest("<method>", "<route>", ctx => {
    ctx.Query("key", "value");
});
```

<a name="section-browsercontext-extensions"></a>
### Extensions

Crowbar provides several extension method to `BrowserContext`.

#### FormsAuth

Supplies a forms authentication cookie with the request.

#### JsonBody

Sets the content type to _application/json_ (overridable) and provides a JSON object as the body of the request.

#### XmlBody

Sets the content type to _application/xml_ (overridable) and provides an XML object as the body of the request.

<a name="section-browserresponse"></a>
BrowserResponse
---------------

The `BrowserResponse` class defines the properties of the HTTP response.

<a name="section-browserresponse-advanced"></a>
### Advanced

The `Advanced` property provides access to various ASP.NET MVC contexts which are collected during the request.

<a name="section-browserresponse-contenttype"></a>
### ContentType

Returns the content type of the request.

<a name="section-browserresponse-headers"></a>
### Headers (faked)

Provides access to the _Location_ header if the response is a redirect.

<a name="section-browserresponse-responsebody"></a>
### ResponseBody

Returns the string representation of the body of the response.

<a name="section-browserresponse-statuscode"></a>
### StatusCode

Returns the HTTP status code of the response.

<a name="section-browserresponse-ascsquery"></a>
### AsCsQuery

Returns a [CsQuery](https://github.com/jamietre/CsQuery) object (similar to a jQuery object) which provides CSS selection and DOM manipulation functionality.

<a name="section-browserresponse-asjson"></a>
### AsJson

Returns a JSON object as a `dynamic` object.

<a name="section-browserresponse-asxml"></a>
### AsXml

Returns an XML object as an `XElement` object.

<a name="section-browserresponse-extensions"></a>
### Extensions

Crowbar provides several extension methods for easing the assertion of the correct behavior of the request.

#### ShouldBeHtml

Asserts that the HTTP status code is 'HTTP Status 200 OK' and that the content type is _text/html_. It provides an optional argument for performing additional assertions on the returned HTML document (`CsQuery`). The `CsQuery` object is also returned from the method.

``` csharp
response.ShouldBeHtml(document => { 
    var main = document["#main"];
    Assert.That(main, Has.Length.EqualTo(1));
});
```

#### ShouldBeJson

Asserts that the HTTP status code is 'HTTP Status 200 OK' and that the content type is _application/json_ (overridable). It provides an optional argument for performing additional assertions on the returned JSON object (`dynamic`). The `dynamic` object is also returned from the method.

``` csharp
response.ShouldBeJson(json => { 
    Assert.That(json.value, Is.EqualTo("Crowbar"));
});
```

#### ShouldBeXml

Asserts that the HTTP status code is 'HTTP Status 200 OK' and that the content type is _application/xml_ (overridable). It provides an optional method for performing additional assertions on the returned XML object (`XElement`). The `XElement` object is also returned from the method.

``` csharp
response.ShouldBeXml(xml => { 
    Assert.That(xml.Value, Is.EqualTo("Crowbar"));
});
```

#### ShouldHaveCookie / ShouldNotHaveCookie

Asserts that the response has or has not a cookie with the specified name (and value).

``` csharp
response.ShouldHaveCookie("<name of cookie>");
```

``` csharp
response.ShouldHaveCookie("<name of cookie>", "<value of cookie");
```

``` csharp
response.ShouldNotHaveCookie("<name of cookie>");
```

#### ShouldHavePermanentlyRedirectedTo

Asserts that the HTTP status code is 'HTTP Status 301 MovedPermanently', that the header _Location_ is defined and that the value of the header is equal to the expected location.

``` csharp
response.ShouldHavePermanentlyRedirectTo("/location");
```

#### ShouldHaveTemporarilyRedirectTo

Asserts that the HTTP status code is 'HTTP Status 302 Found', that the header _Location_ is defined and that the value of the header is equal to the expected location.

``` csharp
response.ShouldHaveTemporarilyRedirectTo("/location");
```
<a name="section-samples"></a>
Samples
-------

The project contains two sample projects: Raven.Web/Raven.Tests and Tool.Web/Tool.Tests. The Raven sample project shows how to implement a custom user-defined proxy and context.

<a name="section-troubleshooting"></a>
Troubleshooting
---------------

Crowbar is built using the ASP.NET MVC 3 assembly. If you're using ASP.NET MVC 4 and experience odd behavior consider adding a binding redirect in the root Web.config.

``` xml
  <configuration>
    <runtime>
      <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
        <dependentAssembly>
          <assemblyIdentity name="System.Web.Mvc" publicKeyToken="31bf3856ad364e35" culture="neutral" />
          <bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="4.0.0.0" />
        </dependentAssembly>
      </assemblyBinding>
   </runtime>
  <configuration>
```

<a name="section-changelog"></a>
Changelog
---------

v0.9.3

* Added the option of specifying a default `BrowserContext` (when creating the `MvcApplication`) that will be applied to every request.
* Additional assert helper, `ShouldNotHaveCookie`, for HTTP cookies.

v0.9.2

* Added `BrowserResponse.RawHttpRequest` which returns a raw string representation of the HTTP request (similar to Fiddler). If the server throws an exception the raw HTTP request will be included in the exception message for easier troubleshooting.
* Assert helpers, `ShouldHaveCookie`, for HTTP cookies.
* Added `Html` to `BrowserLoadContinuation` for easier troubleshooting.
* Added `Cookies` and `Html` to `BrowserRenderContinuation<TContext>` for easier troubleshooting.
* More descriptive error messages for `Browser.Submit()`.

v0.9.1

* `CrowbarController`, `As` (new) and `DelegateExtensions` (new) are now part of the public API.
* Upgraded CsQuery to 1.3.4.
* Changes to the build script, enabled package restore.

v0.9

* Initial public release.