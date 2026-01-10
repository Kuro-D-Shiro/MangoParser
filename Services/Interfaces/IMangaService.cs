using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Data.DTO.ResponseDTOs;

namespace MangoParser.Services.Interfaces
{
    public interface IMangaService
    {
        public Task CreateOrUpdateMangaFromParser(MangaFromParsingDTO dto);
        public Task<IReadOnlyCollection<TrainingDataSetDTO>> GetTrainingDataSet();
    }
}
