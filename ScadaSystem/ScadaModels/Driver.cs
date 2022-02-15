using System.Runtime.Serialization;

namespace ScadaModels
{
    public interface IDriver
    {
        double ReturnValue(string address);
    }
}