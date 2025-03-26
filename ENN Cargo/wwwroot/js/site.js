// Add this to your site.js file
document.addEventListener('DOMContentLoaded', function () {
    // Find all logout buttons
    const logoutButtons = document.querySelectorAll('#logout-button, a[href="/Account/Logout"]');

    logoutButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault(); // Prevent the default GET navigation

            // Get the anti-forgery token if it exists
            let token = '';
            const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            if (tokenElement) {
                token = tokenElement.value;
            }

            // Create a form to submit the POST request
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = '/Account/Logout';
            form.style.display = 'none';

            // Add the anti-forgery token if available
            if (token) {
                const tokenInput = document.createElement('input');
                tokenInput.type = 'hidden';
                tokenInput.name = '__RequestVerificationToken';
                tokenInput.value = token;
                form.appendChild(tokenInput);
            }

            // Add the form to the body and submit it
            document.body.appendChild(form);

            // Set a backup redirect in case form submission fails
            setTimeout(function () {
                window.location.href = '/Home/Index';
            }, 2000); // Redirect after 2 seconds as backup

            // Submit the form
            form.submit();
        });
    });
});