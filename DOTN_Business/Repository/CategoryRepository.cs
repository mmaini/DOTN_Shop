using AutoMapper;
using DOTN_Business.Repository.IRepository;
using DOTN_DataAccess.Data;
using DOTN_Models;

namespace DOTN_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        //dependency injection
        public CategoryRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public CategoryDTO Create(CategoryDTO objDto)
        {
            //pretvaramo DTO u Category
            var obj = _mapper.Map<CategoryDTO, Category>(objDto);

            //dodamo u context
            _dbContext.Add(obj);
            //spremimo promjene u bazu
            _dbContext.SaveChanges();

            //vraćamo DTO pa opet moramo napraviti konverziju
            return _mapper.Map<Category, CategoryDTO>(obj);
        }

        public int Delete(int id)
        {
            var obj = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (obj != null)
            {
                _dbContext.Categories.Remove(obj);
                //vraća koliko se stvarili spremilo - u ovom slučaju će vratiti 1
                return _dbContext.SaveChanges();
            }

            //nismo ništa pobrisali
            return 0;
        }

        public CategoryDTO Get(int id)
        {
            var obj = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (obj != null)
            {
                return _mapper.Map<Category, CategoryDTO>(obj);
            }
            return new CategoryDTO();
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_dbContext.Categories);
        }

        public CategoryDTO Update(CategoryDTO objDto)
        {
            var objFromDb = _dbContext.Categories.FirstOrDefault(x => x.Id == objDto.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = objDto.Name;
                _dbContext.Categories.Update(objFromDb);
                _dbContext.SaveChanges();
                return _mapper.Map<Category, CategoryDTO>(objFromDb);
            }

            return objDto;
        }
    }
}
