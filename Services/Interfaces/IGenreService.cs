using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Data.Models;

namespace MangoParser.Services.Interfaces
{
    public interface IGenreService
    {
        public Task<Genre> CreateGenreFromParser(GenreFromParsingDTO dto);
    }
}
