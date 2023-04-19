using DOTN_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTN_Business.Repository.IRepository
{
	public interface ICategoryRepository
	{
		public CategoryDTO Create(CategoryDTO objDto);
		public CategoryDTO Update(CategoryDTO objDto);
		public int Delete(int id);
		public CategoryDTO Get(int id);
		public IEnumerable<CategoryDTO> GetAll();
	}
}
