using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Data.Models;

namespace MangoParser.Services.Interfaces
{
    public interface ITagService
    {
        public Task<Tag> CreateTagFromParser(TagFromParsingDTO dto);
    }
}
