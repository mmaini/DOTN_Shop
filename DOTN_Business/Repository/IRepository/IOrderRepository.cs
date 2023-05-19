﻿using DOTN_Models;

namespace DOTN_Business.Repository.IRepository
{
	public interface IOrderRepository
	{
		public Task<OrderDTO> Get(int id);
		public Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null);

		public Task<OrderDTO> Create(OrderDTO orderDTO);
		public Task<int> Delete(int id);

		public Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO orderHeaderDTO);
		public Task<OrderHeaderDTO> MarkPaymentSuccessful(int id);
		public Task<bool> UpdateOrderStatus(int orderId, string status);
	}
}
