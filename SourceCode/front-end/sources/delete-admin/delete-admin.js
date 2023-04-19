(function (root) {
//const webServiceUrl = 'https://localhost:7259/account/delete';
const user = {}
user.userID = 0;
user.fName = ''
user.lName = ''
user.address = ''
user.DateTime = System.DateTime.UtcNow
user.GenericIdentity = "Admin User"
const deleteAccount = async () => {
    const confirmation = root.confirm('Are you sure you want to delete this account?');
        if (!confirmation) {
          return;
        }
        try {
            const del = document.getElementById('input-container').value;
            const webServiceUrl = 'https://localhost:7259/account/deletesA/${del}/${user}'
            var request = await axios.delete(webServiceUrl, del, user);
            request.then(response => {
                if (response.status === 200) {
                  alert("Account deleted successfully");
                } else {
                  alert("An error occurred while deleting your account");
                }
              })
        }
        catch(error) {
            console.error('Error: ', error);
            alert("An error occurred while deleting your account");
          };
        
}

document.getElementById("delete-btn").addEventListener("click", deleteAccount);
})(window, window.ajaxClient);