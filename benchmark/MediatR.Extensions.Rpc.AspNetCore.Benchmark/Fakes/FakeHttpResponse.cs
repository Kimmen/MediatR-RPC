using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark.Fakes
{
    public sealed class FakeHttpResponse : HttpResponse
    {


        public override HttpContext HttpContext => throw new NotImplementedException();

        public override int StatusCode { get; set; }

        public override IHeaderDictionary Headers => new HeaderDictionary();

        public override Stream Body { get; set; } = new MemoryStream();
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }

        public override IResponseCookies Cookies => new FakeResponseCookies();

        public override bool HasStarted => false;

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        { 
        }

        public override void Redirect(string location, bool permanent)
        {
        }

        public override Task CompleteAsync()
        {
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override PipeWriter BodyWriter => new FakePipeWriter();
    }

    public sealed class FakeResponseCookies : IResponseCookies
    {
        public void Append(string key, string value)
        {
            
        }

        public void Append(string key, string value, CookieOptions options)
        {
        }

        public void Delete(string key)
        {
        }

        public void Delete(string key, CookieOptions options)
        {
        }
    }

    public sealed class FakePipeWriter : PipeWriter
    {
        public override void Advance(int bytes)
        {
        }

        public override void CancelPendingFlush()
        {
        }

        public override void Complete(Exception exception = null)
        {
        }

        public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default)
        {
            return new ValueTask<FlushResult>(new FlushResult(false, true));
        }

        public override Memory<byte> GetMemory(int sizeHint = 0)
        {
            return new Memory<byte>(new byte[sizeHint]);
        }

        public override Span<byte> GetSpan(int sizeHint = 0)
        {
            return new Span<byte>(new byte[sizeHint]);
        }
    }
}
