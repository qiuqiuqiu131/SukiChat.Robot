using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.Service;

public interface ILoginService
{
    Task Login(string userId, string password);
    Task<DateTime> GetLastLoginTime(string userId);
}

public class LoginService:BaseService,ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public LoginService(IServiceProvider serviceProvider):base(serviceProvider)
    {
        _unitOfWork = _scopedProvider.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
    
    public async Task Login(string userId, string password)
    {
        try
        {
            var userLoginRepository = _unitOfWork.GetRepository<LoginHistory>();
            var history = await userLoginRepository.GetFirstOrDefaultAsync(predicate: d => d.Id.Equals(userId),disableTracking:false);
            if (history == null)
            {
                await userLoginRepository.InsertAsync(new LoginHistory
                    { Id = userId, Password = password, LastLoginTime = DateTime.Now });
            }
            else
            {
                history.Password = password;
                history.LastLoginTime = DateTime.Now;
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // do nothing
        }
    }
    
    public async Task<DateTime> GetLastLoginTime(string userId)
    {
        var userLoginRepository = _unitOfWork.GetRepository<LoginHistory>();
        var history = await userLoginRepository.GetFirstOrDefaultAsync(predicate: d => d.Id.Equals(userId),disableTracking:true);
        return history?.LastLoginTime ?? DateTime.MinValue;
    }
}