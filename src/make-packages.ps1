dotnet pack ".\MediatR.Extensions.Rpc.Core\MediatR.Extensions.Rpc.Core.csproj" -c "Release" --include-symbols
dotnet pack ".\MediatR.Extensions.Rpc.AspNetCore\MediatR.Extensions.Rpc.AspNetCore.csproj" -c "Release" --include-symbols
dotnet pack ".\MediatR.Extensions.Rpc.Functions\MediatR.Extensions.Rpc.Functions.csproj" -c "Release" --include-symbols