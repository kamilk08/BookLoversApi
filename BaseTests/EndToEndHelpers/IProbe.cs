using System.Threading.Tasks;

namespace BaseTests.EndToEndHelpers
{
    public interface IProbe
    {
        bool IsSatisfied();

        Task SampleAsync();
    }
}