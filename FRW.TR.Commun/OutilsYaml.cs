using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FRW.TR.Commun
{
    public static class OutilsYaml
    {

        public static IDeserializer deserializer = new DeserializerBuilder()
             .WithNamingConvention(CamelCaseNamingConvention.Instance)
             .IgnoreUnmatchedProperties()
             .Build();

        public static ISerializer serializer = new SerializerBuilder()
                     .WithNamingConvention(CamelCaseNamingConvention.Instance)
                     .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                     .Build();

        public static T LireFicher<T>(string filename)
        {
            T cfg;
            using (var configFile = new StreamReader(filename))
            {
                cfg = deserializer.Deserialize<T>(configFile);
            }
            return cfg;
        }

        public static T DeserializerString<T>(string configFile)
        {
            return deserializer.Deserialize<T>(configFile);
        }

        public static void EcrireFichier<T>(T value, string filename)
        {
            using (var configFile = new StreamWriter(filename))
            {
                serializer.Serialize(configFile, graph: value!);
            }
        }
    }
}
