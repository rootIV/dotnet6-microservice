using AutoMapper;
using GeekShopping.Product.Api.Data.ValueObjects;
using GeekShopping.Product.Api.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Product.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly MySqlContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(MySqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductVO>> FindAll()
    {
        List<Model.Product> products = await _context.Products.ToListAsync();

        return _mapper.Map<List<ProductVO>>(products);
    }
    public async Task<ProductVO> FindById(long id)
    {
        Model.Product product = await _context.Products.Where(product => product.Id == id).FirstOrDefaultAsync() ??
            new Model.Product();

        return _mapper.Map<ProductVO>(product);
    }
    public async Task<ProductVO> Create(ProductVO vo)
    {
        Model.Product product = _mapper.Map<Model.Product>(vo);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductVO>(product);
    }
    public async Task<ProductVO> Update(ProductVO vo)
    {
        Model.Product product = _mapper.Map<Model.Product>(vo);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductVO>(product);
    }
    public async Task<bool> Delete(long id)
    {
        try
        {
            Model.Product product = await _context.Products.Where(product => product.Id == id).FirstOrDefaultAsync() ??
                new Model.Product();

            if (product.Id <= 0)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
