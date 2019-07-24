# CurrieTechnologies.Razor.PageVisibility
This package provides Blazor applications with access to the browser's [Page Visibility API](https://developer.mozilla.org/en-US/docs/Web/API/Page_Visibility_API)

## This package is for both Server-side and Client-side Blazor. [CurrieTechnologies.Blazor.PageVisibility](https://github.com/Basaingeal/Blazor.PageVisibility) is now deprecated.

## Usage
1) In your Blazor app, add the `CurrieTechnologies.Razor.PageVisibility` [NuGet package](https://www.nuget.org/packages/CurrieTechnologies.Razor.PageVisibility/)

    ```
    Install-Package CurrieTechnologies.Razor.PageVisibility
    ```

2) In your Blazor app's `Startup.cs`, register the 'PageVisibilityService'.

    ```
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddPageVisibility();
        ...
    }
    ```

3) AAdd this script tag in  your root html file (Likely _Host.cshtml for Server-side Blazor or index.html for Client-side Blazor), right under the framework script tag. (i.e `<script src="_framework/blazor.server.js"></script>` for Server-side Blazor or `<script src="_framework/blazor.webassembly.js"></script>` for Client-side Blazor)
  ```html
  <script src="_content/CurrieTechnologies.Razor.PageVisibility/pagevisibility.js"></script>
  ```

3) Now you can inject the PageVisibilityService into any Blazor page and use it like this:

    ```
    @inject PageVisibilityService visibility

    <div>
      <ul>
        @foreach (var vs in viewStates)
        {
          <li>@vs</li>
        }
      </ul>
      @if (listenerId != Guid.Empty)
      {
        <button @onclick="HandleUnsubscibe">Unsubscribe</button>
      }
      else
      {
        <button @onclick="(async () => listenerId = await visibility.OnVisibilityChangeAsync(OnVisibilityChange, this))">
          Resubscribe
        </button>
      }

    </div>

    @code {
      private List<string> viewStates = new List<string>();
      private Guid listenerId = Guid.Empty;

      protected override async Task OnInitAsync()
      {
        viewStates.Add(await visibility.GetVisibilityStateAsync());

        listenerId = await visibility.OnVisibilityChangeAsync(OnVisibilityChange, this);

        await base.OnInitAsync();
      }

      Task OnVisibilityChange(VisibilityInfo visibilityInfo)
      {
        viewStates.Add(visibilityInfo.VisibilityState);
        return Task.CompletedTask;
      }

      async Task HandleUnsubscibe()
      {
        await visibility.RemoveVisibilityChangeCallbackAsync(listenerId);
        listenerId = Guid.Empty;
      }
    }
    ```
