declare let DotNet: any;

const domWindow = window as any;

const namespace = "CurrieTechnologies.Razor.PageVisibility";

const visibilityCallbacks = new Map<string, () => Promise<void>>();

function dispatchVisibiliyChange(
  id: string,
  hidden: boolean,
  visibilityState: string,
): Promise<void> {
  return DotNet.invokeMethodAsync(
    namespace,
    "ReceiveVisibiliyChange",
    id,
    hidden,
    visibilityState,
  );
}

function visibilityCallbackFactory(actionId: string) {
  return (): Promise<void> =>
    dispatchVisibiliyChange(
      actionId,
      document.hidden,
      document.visibilityState,
    );
}

domWindow.CurrieTechnologies = domWindow.CurrieTechnologies || {};
domWindow.CurrieTechnologies.Razor = domWindow.CurrieTechnologies.Razor || {};
domWindow.CurrieTechnologies.Razor.PageVisibility =
  domWindow.CurrieTechnologies.Razor.PageVisibility || {};

domWindow.CurrieTechnologies.Razor.PageVisibility.IsHidden = (): boolean => {
  return document.hidden;
};

domWindow.CurrieTechnologies.Razor.PageVisibility.GetVisibilityState = (): string => {
  return document.visibilityState;
};

domWindow.CurrieTechnologies.Razor.PageVisibility.OnVisibilityChange = (
  actionId: string,
): void => {
  const callback = visibilityCallbackFactory(actionId);
  visibilityCallbacks.set(actionId, callback);
  document.addEventListener("visibilitychange", callback);
};

domWindow.CurrieTechnologies.Razor.PageVisibility.RemoveVisibilityChangeCallback = (
  actionId: string,
): void => {
  const callback = visibilityCallbacks.get(actionId) as () => Promise<void>;
  document.removeEventListener("visibilitychange", callback);
  visibilityCallbacks.delete(actionId);
};
