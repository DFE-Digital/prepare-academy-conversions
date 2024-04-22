//document.addEventListener('DOMContentLoaded', function () {
//   const searchInput = document.getElementById('search-officer');
//   const officers = document.querySelectorAll('.officer-checkbox');

//   console.log("Officers found: ", officers.length); // Check how many officers are found

//   searchInput.addEventListener('keyup', function (e) {
//      const searchValue = e.target.value.toLowerCase();
//      console.log("Searching for: ", searchValue); // See what is being typed

//      officers.forEach(function (item) {
//         const officerName = item.textContent.toLowerCase();
//         console.log("Checking officer: ", officerName); // Check each officer's name

//         if (officerName.includes(searchValue)) {
//            item.style.display = '';
//         } else {
//            item.style.display = 'none';
//         }
//      });
//   });
//});

document.addEventListener('DOMContentLoaded', function () {
   setupFilters();

   function setupFilters() {
      const filterSections = document.querySelectorAll('.govuk-accordion__section');
      filterSections.forEach(section => {
         const searchInput = section.querySelector('.govuk-input');
         const items = section.querySelectorAll('.govuk-checkboxes__item');

         if (searchInput) {
            searchInput.addEventListener('keyup', function (e) {
               const searchValue = e.target.value.toLowerCase();
               filterItems(items, searchValue);
            });
         }
      });
   }

   function filterItems(items, searchValue) {
      items.forEach(function (item) {
         const itemName = item.textContent.toLowerCase();
         if (itemName.includes(searchValue)) {
            item.style.display = '';
         } else {
            item.style.display = 'none';
         }
      });
   }
});



