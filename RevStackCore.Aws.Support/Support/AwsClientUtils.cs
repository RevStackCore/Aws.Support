// Copyright (c) ServiceStack, Inc. All Rights Reserved.
// License: https://raw.github.com/ServiceStack/ServiceStack/master/license.txt

using System.Threading;
using ServiceStack.Text;

namespace RevStackCore.Aws.Support
{
    public static class AwsClientUtils
    {
        public static JsConfigScope GetJsScope()
        {
            return JsConfig.With(excludeTypeInfo: false);
        }

        public static string ToScopedJson<T>(T value)
        {
            using (GetJsScope())
            {
                return JsonSerializer.SerializeToString(value);
            }
        }

        /// <summary>
        /// Sleep using AWS's recommended Exponential BackOff with Full Jitter from:
        /// https://aws.amazon.com/blogs/architecture/exponential-backoff-and-jitter/
        /// </summary>
        /// <param name="retriesAttempted"></param>
        public static void SleepBackOffMultiplier(this int retriesAttempted) => 
            Thread.Sleep(ExecUtils.CalculateFullJitterBackOffDelay(retriesAttempted));
    }
}