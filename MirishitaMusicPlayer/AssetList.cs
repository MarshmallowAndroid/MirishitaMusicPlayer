using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MirishitaMusicPlayer
{
    public class AssetList
    {
        public AssetList(Stream assetListStream)
        {
            object[] deserializedObjects = MessagePackSerializer.Deserialize<object>(assetListStream) as object[];
            var fileDictionary = (deserializedObjects[0] as Dictionary<object, object>).Where(
                a =>
                {
                    string fileName = a.Key.ToString();
                    return fileName.StartsWith("jacket") || fileName.StartsWith("scrobj") || fileName.StartsWith("song3");
                });

            Assets = new Asset[fileDictionary.Count()];

            int index = 0;
            foreach (var file in fileDictionary)
            {
                string fileName = file.Key.ToString();
                var assetInfo = file.Value as object[];

                uint fileSize = 0;
                if (assetInfo[2].GetType() == typeof(ushort))
                    fileSize = (ushort)assetInfo[2];
                else if (assetInfo[2].GetType() == typeof(uint))
                    fileSize = (uint)assetInfo[2];

                Assets[index++] = new(fileName, assetInfo[1].ToString(), fileSize);
            }

            Array.Sort(Assets, (i1, i2) => i1.Name.CompareTo(i2.Name));

            assetListStream.Dispose();
        }

        public Asset[] Assets { get; }
    }
}
