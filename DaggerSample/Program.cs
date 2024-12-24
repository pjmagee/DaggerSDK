using Dagger.Sdk;

var ctr = Client
    .Query()
    .Container()
    .From("alpine")
    .WithExec(["echo", "hello"]);
    
var output = await ctr.StdoutAsync();

Console.WriteLine(output);