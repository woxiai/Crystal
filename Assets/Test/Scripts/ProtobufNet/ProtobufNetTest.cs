using System;
using System.IO;
using UnityEngine;
using ProtoBuf;
using Crystal;
using Crystal.Collections;

namespace Assets.Test.Scripts.ProtobufNet
{
    public class ProtobufNetTest : MonoBehaviour
    {

        private void Awake()
        {
            //ProfilerUtility.BeginSample("Proto");
            var pd = new ProtoDefault() {
                id = 1,
                level = 1,
                skill = "Skill1",
                domaind = 6.6666F,
                st = SkillType.Skill3
            };
            using (var ms = new MemoryStream()) {
                Serializer.Serialize<ProtoDefault>(ms, pd);
                var bytes = ms.GetBuffer();
                Debug.LogFormat("serialize length = {0}", bytes.Length);
                var pd2 = Serializer.Deserialize<ProtoDefault>(ms);
                Debug.LogFormat("serialize pd == pd2 : {0}", pd == pd2);
            }
            //ProfilerUtility.EndSample();

            //ConditionalBuild.ProfilerUtility.BeginSample("bEING");

            //ConditionalBuild.ProfilerUtility.EndSample();

            ProfilerUtility.BeginSample("BeginSample.");
            ProfilerUtility.EndSample();

            for (var i = 100; i >= 0; i--)
            {
                //TestPriorityQueue();
                //TestPriorityQueueForJava();
                //TestXLoss();
            }
        }

        private void Update()
        {
            ProfilerUtility.BeginSample("UpdateProto");
            ProfilerUtility.EndSample();
            //var pd = new ProtoDefault()
            //{
            //    id = 1,
            //    level = 1,
            //    skill = "Skill1",
            //    domaind = 6.6666F,
            //    st = SkillType.Skill3
            //};
            //using (var ms = new MemoryStream())
            //{
            //    Serializer.Serialize<ProtoDefault>(ms, pd);
            //    var bytes = ms.GetBuffer();
            //    Debug.LogFormat("serialize length = {0}", bytes.Length);
            //    var pd2 = Serializer.Deserialize<ProtoDefault>(ms);
            //    Debug.LogFormat("serialize pd == pd2 : {0}", pd == pd2);
            //}
            //ProfilerUtility.EndSample();
        }


        void TestPriorityQueue()
        {
            PriorityQueueSimple<int> queue = new PriorityQueueSimple<int>();
            for (var i = 0; i < 36; i++)
            {
                queue.Push(UnityEngine.Random.Range(-100, 100));
            }
            Debug.LogFormat("Priority Queue Test Begin");
            while (queue.Count > 0)
            {
                Debug.LogFormat("Queue item = {0}", queue.Pop());
            }
            Debug.LogFormat("Priority Queue Test End");
        }

        void TestPriorityQueueForJava()
        {
            PriorityQueue<int> queue = new PriorityQueue<int>();
            Debug.LogFormat("Priority Queue Test Begin");
            var begin = Time.realtimeSinceStartup;
            for (var i = 0; i < 50; i++)
            {
                queue.Enqueue(UnityEngine.Random.Range(-100, 100));
            }
            var delta1 = Time.realtimeSinceStartup - begin;
            begin = Time.realtimeSinceStartup;
            while (queue.Count > 0)
            {
                queue.Dequeue();
                //Debug.LogFormat("Queue item = {0}", queue.Dequeue());
            }
            var delta2 = Time.realtimeSinceStartup - begin;
            Debug.LogFormat("Priority Queue Test End , delta1 = {0}, delta2 = {1}", delta1, delta2);
        }


        void TestXLoss()
        {
            var queue = new PriorityQueue<XLoss>();
            var begin = Time.realtimeSinceStartup;
            for (var i = 0; i < 50; i++)
            {
                var xLoss = new XLoss(UnityEngine.Random.Range(0, 1000), UnityEngine.Random.Range(0, 1000));
                queue.Enqueue(xLoss);
            }
            var delta1 = Time.realtimeSinceStartup - begin;
            begin = Time.realtimeSinceStartup;
            while (queue.Count > 0)
            {
                queue.Dequeue();
            }
            var delta2 = Time.realtimeSinceStartup - begin;
            Debug.LogFormat("Priority Queue Test End , delta1 = {0}, delta2 = {1}", delta1, delta2);
        }

        private class XLoss : IComparable<XLoss>
        {
            public int g;

            public int h;

            public XLoss(int g, int h)
            {
                this.g = g;
                this.h = h;
            }

            public int f => g + h;

            public int CompareTo(XLoss other)
            {
                return this.f - other.f;
            }

            public override string ToString()
            {
                return string.Concat("G = ", g.ToString(), ", H = ", h.ToString(), " F = ", f.ToString());
            }
        }
    }

    [ProtoContract]
    public class ProtoDefault
    {
        [ProtoMember(1)]
        public int id;
        [ProtoMember(2)]
        public int level;
        [ProtoMember(3)]
        public string skill;
        [ProtoMember(4)]
        public float domaind;
        [ProtoMember(5)]
        public SkillType st;
    }

    public enum SkillType
    {
        Skill1,
        Skill2,
        Skill3
    }
}
