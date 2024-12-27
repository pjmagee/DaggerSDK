// dagger run dotnet run

var client = Dagger.Sdk.Client.Query();

// Console.WriteLine(await client.Container().From("alpine").WithExec(["echo", "hello"]).StdoutAsync());

const string dockerfile = """
                          FROM alpine:3.20.0
                          ARG SPAM=spam
                          ENV SPAM=$SPAM
                          CMD printenv
                          """;

Console.WriteLine(await client.Host().IdAsync());

Directory dockerDir = client.Directory().WithNewFile("Dockerfile", dockerfile);

Console.WriteLine(await client.Container().Build(dockerDir, buildArgs: [new ("SPAM", "egg")]).WithExec(args: []).StdoutAsync());
