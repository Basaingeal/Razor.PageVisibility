using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrieTechnologies.Razor.PageVisibility
{
    public class PageVisibilityService
    {
        private readonly IJSRuntime jSRuntime;

        static readonly IDictionary<Guid, EventCallback<VisibilityInfo>> visibilityChangeCallbacks =
           new Dictionary<Guid, EventCallback<VisibilityInfo>>();

        public PageVisibilityService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        /// <summary>
        /// Returns true if the page is in a state considered to be hidden to the user, and false otherwise.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsHiddenAsync()
        {
            var result = await jSRuntime.InvokeAsync<bool?>("CurrieTechnologies.Razor.PageVisibility.IsHidden");
            if (result == null)
            {
                throw new JSException("Visibility not supported");
            }

            return result.Value;
        }

        /// <summary>
        /// A DOMString indicating the document's current visibility state.
        /// <see cref="VisibilityState"/>
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetVisibilityStateAsync()
        {
            var result = await jSRuntime.InvokeAsync<string>("CurrieTechnologies.Razor.PageVisibility.GetVisibilityState");
            if (result == null)
            {
                throw new JSException("Visibility not supported");
            }

            return result;
        }

        /// <summary>
        /// An EventListener providing the code to be called when the visibilitychange event is fired.
        /// </summary>
        /// <param name="visibilityCallback">The action to perform when the visibility changes.</param>
        /// <param name="callingComponent">Pass in 'this' from the calling component.</param>
        /// <returns>A GUID that can be used to clear the event callback.</returns>
        public async Task<Guid> OnVisibilityChangeAsync(Func<VisibilityInfo, Task> visibilityCallback, object callingComponent)
        {
            EventCallback<VisibilityInfo> eventCallback = EventCallback.Factory.Create(callingComponent, visibilityCallback);
            return await AttachCallbackToDomAsync(eventCallback);
        }

        /// <summary>
        /// An EventListener providing the code to be called when the visibilitychange event is fired.
        /// </summary>
        /// <param name="visibilityCallback">The action to perform when the visibility changes.</param>
        /// <param name="callingComponent">Pass in 'this' from the calling component.</param>
        /// <returns>A GUID that can be used to clear the event callback.</returns>
        public async Task<Guid> OnVisibilityChangeAsync(Action<VisibilityInfo> visibilityCallback, object callingComponent)
        {
            EventCallback<VisibilityInfo> eventCallback = EventCallback.Factory.Create(callingComponent, visibilityCallback);
            return await AttachCallbackToDomAsync(eventCallback);
        }

        private async Task<Guid> AttachCallbackToDomAsync(EventCallback<VisibilityInfo> eventCallback)
        {
            var actionId = Guid.NewGuid();
            visibilityChangeCallbacks.Add(actionId, eventCallback);
            await jSRuntime.InvokeAsync<string>("CurrieTechnologies.Razor.PageVisibility.OnVisibilityChange", actionId);
            return actionId;
        }

        [JSInvokable]
        public static async Task ReceiveVisibiliyChange(string id, bool hidden, string visibilityState)
        {
            var actionId = Guid.Parse(id);
            if (!visibilityChangeCallbacks.ContainsKey(actionId))
            {
                return;
            }
            var action = visibilityChangeCallbacks.First(x => x.Key == actionId).Value;
            var visibilityInfo = new VisibilityInfo
            {
                Hidden = hidden,
                VisibilityState = visibilityState
            };
            await action.InvokeAsync(visibilityInfo);
        }

        /// <summary>
        /// Removes a callback set with OnVisibilityChangeAsync.
        /// </summary>
        /// <param name="callbackId">The GUID of the callback obtained when setting the listener.</param>
        /// <returns></returns>
        public async Task RemoveVisibilityChangeCallbackAsync(Guid callbackId)
        {
            if (visibilityChangeCallbacks.ContainsKey(callbackId))
            {
                visibilityChangeCallbacks.Remove(callbackId);
            }

            await jSRuntime.InvokeAsync<string>("CurrieTechnologies.Razor.PageVisibility.RemoveVisibilityChangeCallback", callbackId);
        }

        /// <summary>
        /// Removes a callback set with OnVisibilityChangeAsync.
        /// </summary>
        /// <param name="callbackId">The GUID of the callback obtained when setting the listener.</param>
        /// <returns></returns>
        public Task RemoveVisibilityChangeCallbackAsync(string callbackId)
        {
            var callbackIdGuid = Guid.Parse(callbackId);
            return RemoveVisibilityChangeCallbackAsync(callbackIdGuid);
        }
    }
}
