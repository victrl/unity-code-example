namespace App.Core.AppServices.ReadWriteSaveServices
{
    public struct SaveData : ISaveData
    {
        public string RawData { get; private set; }

        public SaveData(string rawData)
        {
            RawData = rawData;
        }
    }
}