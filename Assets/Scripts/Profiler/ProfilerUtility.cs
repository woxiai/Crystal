using System.Diagnostics;
using UnityEngine.Profiling;
using UnityEngine;

namespace Crystal
{
    public class ProfilerUtility 
    {
        [Conditional("Profiler")]
        public static void BeginSample(string sampleTag) {
            Profiler.BeginSample(sampleTag);
            UnityEngine.Debug.LogFormat("BeginSample tag = {0}", sampleTag);
        }

        [Conditional("Profiler")]
        public static void EndSample()
        {
            Profiler.EndSample();
            UnityEngine.Debug.LogFormat("EndSample");
        }
    }
}
