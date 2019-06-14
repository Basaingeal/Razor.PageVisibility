# CurrieTechnologies.Razor.PageVisibility
This package provides Blazor applications with access to the browser's [Page Visibility API](https://developer.mozilla.org/en-US/docs/Web/API/Page_Visibility_API)

## This package is for Server-side Blazor only. For Client-side Blazor use [CurrieTechnologies.Blazor.PageVisibility](https://github.com/Basaingeal/Blazor.PageVisibility)

## Usage
1) In your Blazor app, add the `CurrieTechnologies.Razor.PageVisibility` [NuGet package](https://www.nuget.org/packages/CurrieTechnologies.Razor.PageVisibility/)

    ```
    Install-Package CurrieTechnologies.Razor.PageVisibility
    ```

1) In your Blazor app's `Startup.cs`, register the 'PageVisibilityService'.

    ```
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddPageVisibility();
        ...
    }
    ```

2) Add this script tag in  your root html file (Likely _Host.cshtml), right under the `<script src="_framework/blazor.server.js"></script>` tag.
```html
<script src="_content/currietechnologiesrazorpagevisibility/pagevisibility.js"></script>
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
        <button @onclick="@HandleUnsubscibe">Unsubscribe</button>
      }
      else
      {
        <button @onclick="@(async () => listenerId = await visibility.OnVisibilityChangeAsync(OnVisibilityChange, this))">
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
