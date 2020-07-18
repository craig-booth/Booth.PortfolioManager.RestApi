﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace Booth.PortfolioManager.RestApi.Serialization
{
    public interface IRestClientSerializer
    {
        string Serialize(object obj);
        string Serialize<T>(T obj);
        void Serialize(Stream stream, object obj);
        void Serialize<T>(Stream stream, T obj);

        T Deserialize<T>(string source);
        T Deserialize<T>(Stream stream);
    }
    public class RestClientSerializer : IRestClientSerializer
    {
        private readonly JsonSerializer _Serializer;
        public RestClientSerializer()
        {
            _Serializer = JsonSerializer.Create(RestSerializerSettings.Settings);
        }

        public T Deserialize<T>(string source)
        {
            using (var textReader = new StringReader(source))
            {
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    return _Serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var textReader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    var obj = _Serializer.Deserialize<T>(jsonReader);

                    return obj;
                }
            }
        }

        public string Serialize(object obj)
        {
            var textWriter = new StringWriter();
            _Serializer.Serialize(textWriter, obj);

            return textWriter.ToString();
        }

        public string Serialize<T>(T obj)
        {
            var textWriter = new StringWriter();
            _Serializer.Serialize(textWriter, obj, typeof(T));

            return textWriter.ToString();
        }

        public void Serialize(Stream stream, object obj)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                _Serializer.Serialize(streamWriter, obj);
            }
        }

        public void Serialize<T>(Stream stream, T obj)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                _Serializer.Serialize(streamWriter, obj, typeof(T));
            }
        }
    }

    public static class RestSerializerSettings
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                var settings = new JsonSerializerSettings();

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Converters.Add(new StringEnumConverter() { CamelCaseText = true });
                settings.Converters.Add(new DateJsonConverter());
                settings.Converters.Add(new TimeJsonConverter());
                settings.Converters.Add(new TransactionConverter());
                settings.Converters.Add(new CorporateActionConverter());

                return settings;
            }
        }
    }
}
