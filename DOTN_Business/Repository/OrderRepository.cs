using AutoMapper;
using DOTN_Business.Repository.IRepository;
using DOTN_Common;
using DOTN_DataAccess;
using DOTN_DataAccess.Data;
using DOTN_DataAccess.ViewModel;
using DOTN_Models;
using Microsoft.EntityFrameworkCore;

namespace DOTN_Business.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public OrderRepository(AppDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}
		public async Task<OrderDTO> Create(OrderDTO orderDTO)
		{
			try
			{
				var obj = _mapper.Map<OrderDTO, Order>(orderDTO);
				//dodajemo zaglavlje
				_dbContext.OrderHeaders.Add(obj.OrderHeader);
				await _dbContext.SaveChangesAsync();

				//dodijeli id detaljima
				foreach (var detail in obj.OrderDetails)
				{
					detail.OrderHeaderId = obj.OrderHeader.Id;
				}
				_dbContext.OrderDetails.AddRange(obj.OrderDetails);
				await _dbContext.SaveChangesAsync();

				return new OrderDTO()
				{
					OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
					OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(obj.OrderDetails).ToList()
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<int> Delete(int id)
		{
			var objHeader = await _dbContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);
			if (objHeader != null)
			{
				IEnumerable<OrderDetail> objDetails = _dbContext.OrderDetails.Where(x => x.OrderHeaderId == id);
				_dbContext.OrderDetails.RemoveRange(objDetails);
				_dbContext.OrderHeaders.Remove(objHeader);
				return await _dbContext.SaveChangesAsync();
			}

			return 0;
		}

		public async Task<OrderDTO> Get(int id)
		{
			Order order = new Order()
			{
				OrderHeader = _dbContext.OrderHeaders.FirstOrDefault(x => x.Id == id),
				OrderDetails = _dbContext.OrderDetails.Where(x => x.OrderHeaderId == id).ToList()
			};

			if (order != null)
			{
				return _mapper.Map<Order, OrderDTO>(order);
			}

			return new OrderDTO();
		}

		public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
		{
			List<Order> orderFromDb = new List<Order>();
			IEnumerable<OrderHeader> orderHeaderList = _dbContext.OrderHeaders;
			IEnumerable<OrderDetail> orderDetailList = _dbContext.OrderDetails;

			foreach (var header in orderHeaderList)
			{
				Order order = new Order()
				{
					OrderHeader = header,
					OrderDetails = orderDetailList.Where(x => x.OrderHeaderId == header.Id).ToList()
				};
				orderFromDb.Add(order);
			}

			//todo: filtering
			return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orderFromDb);

		}

		public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id)
		{
			var data = await _dbContext.OrderHeaders.FindAsync(id);
			if (data == null)
			{
				return new OrderHeaderDTO();
			}
			if (data.Status == SD.Status_Pending)
			{
				data.Status = SD.Status_Confirmed;
				await _dbContext.SaveChangesAsync();
				return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
			}
			return new OrderHeaderDTO();
		}

		public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO orderHeaderDTO)
		{
			if (orderHeaderDTO != null)
			{
				var orderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(orderHeaderDTO);
				_dbContext.OrderHeaders.Update(orderHeader);
				await _dbContext.SaveChangesAsync();
				return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
			}

			return new OrderHeaderDTO();
		}

		public async Task<bool> UpdateOrderStatus(int orderId, string status)
		{
			var data = await _dbContext.OrderHeaders.FindAsync(orderId);
			if (data == null) { return false; }

			data.Status = status;
			if (status == SD.Status_Shipped)
			{
				data.ShippingDate = DateTime.Now;
			}
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
