using System;

namespace App.Core.AppServices.ReadWriteSaveServices
{
    public interface IReadWriteSaveService : IService
    {
        void ReadSaveData(Action<ISaveData> onComplete);
        void WriteSaveData(string data, Action<bool> onComplete);
    }

    public interface ISaveData
    {
        string RawData { get; }
    }
}