using Visualization;
using Moq;
using NUnit.Framework;
using Visualization;

namespace ViewModelTests
{
    public class AccountViewModelTest
    {
        private AccountViewModel _accountViewModel;

        [SetUp]
        public void Setup()
        {
            this._accountViewModel = new AccountViewModel();
        }
        
        [Test]
        public void TestTheCampingCustomerOverview()
        {
            Account account = new Account("1", "admin", "nimda", "0");
            Address address = new Address("1", "testAddress", "testPostalCode", "testPlace");
            CampingCustomer campingCustomer = new CampingCustomer("1", account, address, "19/10/21", "testPhoneNumber", "testFirstName", "testLastName");

            CurrentUser.SetCurrentUser(account, campingCustomer);
            
            Assert.AreEqual("testPostalCode testPlace", this._accountViewModel.Address);
            Assert.AreEqual("testFirstName testLastName", this._accountViewModel.Name);
        }
    }
}