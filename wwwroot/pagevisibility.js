"use strict";

var domWindow = window;
var namespace = "CurrieTechnologies.Razor.PageVisibility";
var visibilityCallbacks = new Map();

function dispatchVisibiliyChange(id, hidden, visibilityState) {
  return DotNet.invokeMethodAsync(namespace, "ReceiveVisibiliyChange", id, hidden, visibilityState);
}

function visibilityCallbackFactory(actionId) {
  return function () {
    return dispatchVisibiliyChange(actionId, document.hidden, document.visibilityState);
  };
}

domWindow.CurrieTechnologies = domWindow.CurrieTechnologies || {};
domWindow.CurrieTechnologies.Razor = domWindow.CurrieTechnologies.Razor || {};
domWindow.CurrieTechnologies.Razor.PageVisibility = domWindow.CurrieTechnologies.Razor.PageVisibility || {};

domWindow.CurrieTechnologies.Razor.PageVisibility.IsHidden = function () {
  return document.hidden;
};

domWindow.CurrieTechnologies.Razor.PageVisibility.GetVisibilityState = function () {
  return document.visibilityState;
};

domWindow.CurrieTechnologies.Razor.PageVisibility.OnVisibilityChange = function (actionId) {
  var callback = visibilityCallbackFactory(actionId);
  visibilityCallbacks.set(actionId, callback);
  document.addEventListener("visibilitychange", callback);
};

domWindow.CurrieTechnologies.Razor.PageVisibility.RemoveVisibilityChangeCallback = function (actionId) {
  var callback = visibilityCallbacks.get(actionId);
  document.removeEventListener("visibilitychange", callback);
  visibilityCallbacks.delete(actionId);
};