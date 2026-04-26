using NugetGymphonyAGM.Models;

namespace ApiGymphony.Models
{
    public class EntrenadorDTO
    {
        public SocioDTO Usuario { get; set; }
        public List<int> DiasSemana { get; set; }
        public List<TimeOnly> HorasInicio { get; set; }
        public List<TimeOnly> HorasFin { get; set; }
    }
}
