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
    * [Default HttpPayload](#section-default-httppayload)
* [Client](#section-client)
    * [Extensions](#section-client-extensions)
* [HttpPayload](#section-httppayload)
    * [AjaxRequest](#section-httppayload-ajaxrequest)
    * [Body](#section-httppayload-body)
    * [Cookie](#section-httppayload-cookie)
    * [FormValue](#section-httppayload-formvalue)
    * [Header](#section-httppayload-header)
    * [HttpRequest](#section-httppayload-httprequest)
    * [HttpsRequest](#section-httppayload-httpsrequest)
    * [Query](#section-httppayload-query)
    * [Extensions](#section-httppayload-extensions)
* [ClientResponse](#section-clientresponse)
    * [Advanced](#section-clientresponse-advanced)
    * [ContentType](#section-clientresponse-contenttype)
    * [Headers](#section-clientresponse-headers)
    * [ResponseBody](#section-clientresponse-responsebody)
    * [StatusCode](#section-clientresponse-statuscode)
    * [AsCsQuery](#section-clientresponse-ascsquery)
    * [AsJson](#section-clientresponse-asjson)
    * [AsXml](#section-clientresponse-asxml)
    * [Extensions](#section-clientresponse-extensions)
* [Samples](#section-samples)
* [Troubleshooting](#section-troubleshooting)
* [Known Issues](#section-known-issues)
* [Changelog](#section-changelog)

<a name="section-mvcapplication"></a>
MvcApplication
--------------

The `MvcApplication` class (both generic and non-generic) is the heart of Crowbar and represents a proxy to the ASP.NET MVC project. Please note that creating the `MvcApplication` instance is a time-consuming process and should preferably only be done once, e.g., in a test base class.

An instance of `MvcApplication` can be created using the `MvcApplication.Create()` facade methods.

``` csharp
public class MvcApplicationTests
{
    private static readonly MvcApplication Application =
        MvcApplication.Create("<ASP.NET MVC project>");

    [Test]
    public void Should_return_html()
    {
        Application.Execute(client =>
        {
            var response = client.Get("/route");
            response.ShouldBeHtml();
        });        
    }
}
```

``` csharp
public class MvcApplicationTests
{
    protected static readonly MvcApplication<MvcHttpApplication, AppProxyContext> Application = 
        MvcApplication.Create<MvcHttpApplication, AppProxy, AppProxyContext>("<ASP.NET MVC project>");

    [Test]
    public void Should_return_html()
    {
        Application.Execute((client, context) =>
        {
            // Use any state in the context to bootstrap your test.

            var response = client.Get("/route");
            response.ShouldBeHtml();
        });
    }
}
```

By default, Crowbar, `WebProjectPathProvider`, will attempt to locate the web project in the base directory of the current `AppDomain`, if the web project is not found in the base directory Crowbar will move up one directory at a time until it reaches the root. If the web project is not found Crowbar will throw an exception. Crowbar, `WebConfigPathProvider`, will look for Web.config in the base directory of the current `AppDomain`. Should your web project and/or Web.config be located in a different location you will have to provide your own implementation of `IPathProvider` and pass it/them to `MvcApplicationFactory.Create()`.

``` csharp
public static class MvcApplicationFacade
{
    public class CustomWebProjectPathProvider : IPathProvider
    {
        public string GetPhysicalPath()
        {
            // return the path to your web project.
        }
    }

    public class CustomWebConfigPathProvider : IPathProvider
    {
        public string GetPhysicalPath()
        {
            // return the path to your Web.config.
        }
    }

    public static MvcApplication Create()
    {
        return MvcApplicationFactory.Create(new CustomWebProjectPathProvider(), new CustomWebConfigPathProvider());
    }
}

```

<a name="section-mvcapplication-context"></a>
### User-Defined Context

In order to be able to pass state from the server (the MVC application) to the test case it is possible to define a user-defined context. The context must implement `IDisposable`. The context will be created for each test and will be disposed after the test has been run.

``` csharp
public class AppProxyContext : IDisposable
{
    // Add state from the server.

    public void Dispose()
    {
        // Will be disposed after the test case has been run.
    }
}
```

<a name="section-mvcapplication-proxy"></a>
### User-Defined Proxy

In order to initialize the user-defined context we need to defined our own proxy. This is done by deriving from `MvcApplicationProxyBase<THttpApplication, TContext>`.

``` csharp
public class AppProxy : MvcApplicationProxyBase<MvcHttpApplication, UserDefinedContext>
{
    protected override void OnApplicationStart(MvcHttpApplication application)
    {
         // This method will run once; after the application has been initialized by prior to any test.
    }	

    protected override UserDefinedContext CreateContext(MvcHttpApplication application, string testBaseDirectory)
    {
		// This method will run before each test.

        return new UserDefinedContext
        {
            // Set any state.
        };
    }
}
```

<a name="section-mvcapplication-config"></a>
### User-Supplied Web.config

By default Crowbar tries to use the Web.config defined in the ASP.NET MVC project, even though this is somewhat instable at times (the configuration file has not been replaced in the pre-initialization phase, i.e., WebActivator.PreApplicationStartMethod). To counter any problems with the configuration of the MVC project it is possible and highly recommended to use a custom configuration file. The name of the custom configuration file can be supplied as the second argument to `MvcApplication.Create()`. The custom configration file should be defined in the test project. Please note that the configuration file must be copied to the output directory by setting _Copy to Output Directory_ to either _Copy always_ or _Copy if newer_.

``` csharp
private static readonly MvcApplication Application =
    MvcApplication.Create("<ASP.NET MVC project>", "Web.Custom.config");
```

<a name="section-default-httppayload"></a>
### Default HttpPayload

It is possible to define default `HttpPayload` settings for each request by supplying a third argument to `MvcApplication.Create()`. These settings will be applied to the `HttpPayload` before the request-specific settings. See the section on [HttpPayload](#section-httppayload) for available options.

``` csharp
[Serializable]
public class HttpPayloadDefaults : IHttpPayloadDefaults
{
	public void ApplyTo(HttpPayload payload)
	{
		payload.HttpsRequest();
	}
}

private static readonly MvcApplication Application =
    MvcApplication.Create("<ASP.NET MVC project>", "Web.Custom.config", new HttpPayloadDefaults());
```

<a name="section-client"></a>
Client
-------

An instance of the `Client` class enables us to interact with an ASP.NET MVC application by performing requests.

``` csharp
var response = client.PerformRequest("GET", "/route");
```

The `PerformRequest` method has an optional third argument which let us customize the request.

``` csharp
var response = client.PerformRequest("GET", "/route", payload => {
    // Customize the request (HttpPayload object).
});
```

For convenience, the most common HTTP methods have their own methods: `Delete`, `Get`, `Post`, `Put`, which delegate to `PerformRequest`. `PerformRequest` returns an instance of `ClientResponse`.

<a name="section-client-extensions"></a>
### Extensions

#### Submit / AjaxSubmit

The `Submit` and `AjaxSubmit` extensions try to mimic the browser form submission behavior. This is done by extracting the form action, the form method, any form values for the supplied HTML and performing the request based on these values.

``` csharp
var response = client.Submit("<form action='/route' method='post'>...</form>", model);
```

#### Load
Loads HTML from the server using GET and forwards it to either `Submit` or `AjaxSubmit`.

```csharp
var continuation = client.Load("/path/to/form", payload => { // Modify the GET request. });
var response = continuation.Submit(model);
```

#### Render
Renders HTML by reading it from disk and forwards it to either `Submit` or `AjaxSubmit`. The first argument can be either a string (the name of the view) or a `PartialViewContext` object. By using the partial view context it is possible to state whether client validation and/or unobtrusive JavaScript is enabled and set the principal that should be used when rendering the view which is important when the form is rendered with an anti-forgery request token.

``` csharp
var continuation = client.Render("~/path/to/form.cshtml", model);
var response = continuation.Submit();
```

``` csharp
var context = new PartialViewContext("~/path/to/form.cshtml")
{
    ClientValidationEnabled = true,
    UnobtrusiveJavaScriptEnabled = true
};

context.SetFormsAuthPrincipal("username");

var response = client.Render(context, model).Submit();
```

<a name="section-httppayload"></a>
HttpPayload
-----------

The `HttpPayload` class defines the HTTP payload of the HTTP request. As previously stated `Client.PerformRequest()` provides us with the opportunity to customize the HTTP request.

<a name="section-httppayload-ajaxrequest"></a>
### AjaxRequest

To mark the request as an AJAX request call `AjaxRequest()`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    payload.AjaxRequest();
});
```

<a name="section-httppayload-body"></a>
### Body

To set the body of the request use `Body(string body, string contentType`). The second parameter is optional (the default content type is _application/octet-stream_).

<a name="section-httppayload-cookie"></a>
### Cookie

To supply a cookie with the request use `Cookie(HttpRequest cookie)`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    HttpCookie = ...
    payload.Cookie(cookie);
});
```

<a name="section-httppayload-formvalue"></a>
### FormValue

To provide a form value with the request use `FormValue(string key, string value)`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    payload.FromValue("key", "value");
});
```

<a name="section-httppayload-header"></a>
### Header

To provide an HTTP header with the request use `Header(string name, string value)`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    payload.Header("name", "value");
});
```

<a name="section-httppayload-httprequest"></a>
### HttpRequest

To mark the request as using the HTTP protocol (the default value) use `HttpRequest()`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    payload.HttpRequest();
});
```

<a name="section-httppayload-httpsrequest"></a>
### HttpsRequest

To mark the request as using the HTTPS protocol use `HttpsRequest()`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    payload.HttpsRequest();
});
```

<a name="section-httppayload-query"></a>
### Query

To provide a query string entry with the request use `Query(string key, string value)`.

``` csharp
var response = client.PerformRequest("<method>", "<route>", payload => {
    payload.Query("key", "value");
});
```

<a name="section-httppayload-extensions"></a>
### Extensions

Crowbar provides several extension method to `HttpPayload`.

#### FormsAuth

Supplies a forms authentication cookie with the request.

#### JsonBody

Sets the content type to _application/json_ (overridable) and provides a JSON object as the body of the request.

#### XmlBody

Sets the content type to _application/xml_ (overridable) and provides an XML object as the body of the request.

<a name="section-clientresponse"></a>
ClientResponse
---------------

The `ClientResponse` class defines the properties of the HTTP response.

<a name="section-clientresponse-advanced"></a>
### Advanced

The `Advanced` property provides access to various ASP.NET MVC context objects which are collected during the request.

<a name="section-clientresponse-contenttype"></a>
### ContentType

Returns the content type of the request.

<a name="section-clientresponse-headers"></a>
### Headers (faked)

Provides access to the _Location_ header if the response is a redirect.

<a name="section-clientresponse-responsebody"></a>
### ResponseBody

Returns the string representation of the body of the response.

<a name="section-clientresponse-statuscode"></a>
### StatusCode

Returns the HTTP status code of the response.

<a name="section-clientresponse-ascsquery"></a>
### AsCsQuery

Returns a [CsQuery](https://github.com/jamietre/CsQuery) object (similar to a jQuery object) which provides CSS selection and DOM manipulation functionality.

<a name="section-clientresponse-asjson"></a>
### AsJson

Returns a JSON object as a `dynamic` object.

<a name="section-clientresponse-asxml"></a>
### AsXml

Returns an XML object as an `XElement` object.

<a name="section-clientresponse-extensions"></a>
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

The project contains several sample applications (which doubles as sanity test projects).

* Crowbar.Demo.Mvc is an MVC 4 application with no database server.
* Crowbar.Demo.Mvc.Async is an MVC 4 application which demonstrates the use of asynchronous action methods.
* Crowbar.Demo.Mvc.NHibernate is an MVC 4 application which uses NHibernate and a SQLite database server.
* Crowbar.Demo.Mvc.Raven is an MVC 4 application which uses a RavenDB database server.
* Crowbar.Demo.Mvc.WebApi is an MVC 4 application with a WebApi.

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

<a name="section-known-issues"></a>
Known Issues
------------

Tests exercising asynchronous action methods, `public async Task<ActionResult> ActionAsync() { ... }`, will fail when `<httpRuntime targetFramework="4.5" />` is defined in Web.config.

<a name="section-changelog"></a>
Changelog
---------

v0.10 

* Breaking change: The `ProxyBase` hierarchy has been re-written. The most notably change is that classes deriving from `ProxyBase` takes a generic argument `THttpApplication`.
* The `OnApplicationStart` method was added to `ProxyBase`. This method is called after the application has been started but prior to any test case.
* Breaking change: the `BrowserContext` class has been renamed `HttpPayload`.
* Breaking change: the `Browser*` classes (`Browser`, `BrowserResponse` etc) has been renamed `Client*` (`Client`, `ClientResponse` etc).
* Breaking change: the parameter for the default HTTP payload settings is now an interface instead of a delegate. The use of a delegate forced a second initialization of the proxy.

v0.9.6

* Added `IPathProvider`, `WebProjectPathProvider` and `WebConfigPathProvider`.
* The `MvcApplicationFactory` has been made public. In combination with the introduction of `IPathProvider` more advanced scenarios for locating the web project and Web.config are now supported.

v0.9.5

* It is possible to specify a form selector for `Browser.Submit()`. Previously the first form in the HTML chunk was chosen.
* Breaking change: `CrowbarController` has been moved to the root namespace. This fixes a previous oversight when the class was made part of the public API.
* Breaking change: `PartialViewContext` has been renamed `CrowbarViewContext`.
* `CrowbarViewContext` can now find both partial and non-partial views. Any view name starting with an underscore (_) is considered to be a partial view. It is possible to override this behavior in `CrowbarViewContext.FindViewEngineResult()` should the default approach of finding the view not be to your satisfaction.
* The settings `ClientValidationEnabled` and `UnobtrusiveJavaScriptEnabled` in `CrowbarViewContext` are now read from Web.config. Previously they were set to `true` by default.
* Added `AreaName` and `ControllerName` to `CrowbarViewContext` for better control over route data.

v0.9.4

* `ShouldBeHtml`, `ShouldBeJson` and `ShouldBeXml` no longer assumes a HTTP Status 200 OK response.

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