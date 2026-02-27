import * as signalR from "@microsoft/signalr"

export function createHubConnection(url, withAuth = false) {
  const options = {}

  if (withAuth) {
    options.accessTokenFactory = () => {
      return localStorage.getItem("jwt")
    }
  }

  return new signalR.HubConnectionBuilder()
    .withUrl(url, options)
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Warning)
    .build()
}
