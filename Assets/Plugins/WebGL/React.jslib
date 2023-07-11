mergeInto(LibraryManager.library, {
  Ready: function () {
    window.dispatchReactUnityEvent("Ready");
  },
});