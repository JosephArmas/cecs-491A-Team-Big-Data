const webServiceUrl = 'https://localhost:7259/account/delete';
const deleteAccount = async () => {
    const confirmation = window.confirm('Are you sure you want to delete this account?');
        if (!confirmation) {
          return;
        }
        try {
            var request = axios.post(webServiceUrl);
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