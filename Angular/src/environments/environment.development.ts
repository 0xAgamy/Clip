export const environment = {
  production: false,
  // Empty on purpose: dev requests go to /api/... and are forwarded to the API
  // by proxy.conf.json, so the browser never talks to the dev certificate directly.
  apiBaseUrl: '',
};
