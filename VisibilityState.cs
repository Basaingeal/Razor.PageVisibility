namespace CurrieTechnologies.Razor.PageVisibility
{
    /// <summary>
    /// A DOMString indicating the document's current visibility state
    /// </summary>
    public static class VisibilityState
    {
        /// <summary>
        /// The page content may be at least partially visible. In practice this means that the page is the foreground tab of a non-minimized window.
        /// </summary>
        public const string Visible = "visible";

        /// <summary>
        /// The page's content is not visible to the user, either due to the document's tab being in the background or part of a window that is minimized, or because the device's screen is off.
        /// </summary>
        public const string Hidden = "hidden";

        /// <summary>
        /// <para>The page's content is being prerendered and is not visible to the user. A document may start in the prerender state, but will never switch to this state from any other state, since a document can only prerender once.</para>
        /// <para>Note: Not all browsers support prerendering.</para>
        /// </summary>
        public const string Prerender = "prerender";

        /// <summary>
        /// <para>The page is in the process of being unloaded from memory.</para>
        /// <para>Note: Not all browsers support the unloaded value.</para>
        /// </summary>
        public const string Unloaded = "unloaded";
    }
}
