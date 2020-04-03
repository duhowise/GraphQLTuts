using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaOrder.Business.Helpers;
using PizzaOrder.Business.Models;
using PizzaOrder.Data;

namespace PizzaOrder.Business.Services
{
    public class OrderDetailService: IOrderDetailService
    {
        private readonly PizzaDbContext _context;
        private readonly IEventService _eventService;

        public OrderDetailService(PizzaDbContext context,IEventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllNewOrdersAsync()
        {
            return await _context.OrderDetails
                .Where(x => x.OrderStatus == OrderStatus.Created)
                .ToListAsync();
        }

        

        public async Task<OrderDetail> GetOrderDetailAsync(int orderId)
        {
            return await _context.OrderDetails.FindAsync(orderId);
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            _eventService.CreateOrderEvent(new EventDataModel(orderDetail.Id));
            return orderDetail;
        }

        public async Task<OrderDetail> UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderId);
            if (orderDetail != null)
            {
                orderDetail.OrderStatus = status;
                await _context.SaveChangesAsync();
                _eventService.StatusUpdateEvent(new EventDataModel(orderId,status));
            }

            return orderDetail;
        }

        public async Task<PagedResponse<OrderDetail>> GetCompletedOrdersAsync(PagedRequest request)
        {
            var filterQuery = _context.OrderDetails.Where(x => x.OrderStatus == OrderStatus.Delivered);

            #region Obtain Nodes

            var dataQuery = filterQuery;
            if (request.First.HasValue)
            {
                if (!string.IsNullOrEmpty(request.After))
                {
                    var lastId = CursorHelper.FromCursor(request.After);
                    dataQuery = dataQuery.Where(x => x.Id > lastId);
                }

                dataQuery = dataQuery.Take(request.First.Value);
            }
            switch (request.OrderBy?.Field)
            {
                case CompletedOrderSortingFields.Address:
                    dataQuery = (request.OrderBy.SortingDirection == SortingDirection.DESC)
                        ? dataQuery.OrderByDescending(x => x.AddressLine1) : dataQuery.OrderBy(x => x.AddressLine1);
                    break;
                case CompletedOrderSortingFields.Amount:
                    dataQuery = (request.OrderBy.SortingDirection == SortingDirection.DESC)
                        ? dataQuery.OrderByDescending(x => x.Amount) : dataQuery.OrderBy(x => x.Amount);
                    break;
                case CompletedOrderSortingFields.Id:
                    break;
                case null:
                    break;
                default:
                    dataQuery = (request?.OrderBy?.SortingDirection == SortingDirection.DESC)
                        ? dataQuery.OrderByDescending(x => x.Id) : dataQuery.OrderBy(x => x.Id);
                    break;
            }
            var nodes = await dataQuery.ToListAsync();
            

            #endregion



            #region Obtain Flags

            var maxId = nodes.Max(x => x.Id);
            var minId = nodes.Min(x => x.Id);
            var hasNextPage =await filterQuery.AnyAsync(x => x.Id > maxId);
            var hasPreviousPage =await filterQuery.AnyAsync(x => x.Id<minId);
            var totalCount = await filterQuery.CountAsync();

            #endregion

            return new PagedResponse<OrderDetail>
            {
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
                Nodes = nodes,
                TotalCount = totalCount
            };
        }


    }
}