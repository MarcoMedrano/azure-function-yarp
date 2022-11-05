# Yarp running in a azure function

Using _Microsoft.AspNetCore.TestHost_ to redirect requests to YARP, then yarp can redirect to whatever is in the configuration file.

```pwshell
func start
```

### Credits
- [Run AspnetCore in Azure Function](https://blog.wille-zone.de/post/serverless-webapi-hosting-aspnetcore-webapi-in-azure-functions/)
- [Use Yarp](https://swimburger.net/blog/dotnet/use-yarp-to-host-client-and-api-server-on-a-single-origin)
