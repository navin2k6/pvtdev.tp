
$(function () {
    var availableTags = [
              "ActionScript",
              "Agile",
              "AppleScript",
              "AJAX",
              "Andriod",
              "Asp",
              "Asp.Net",
              "Asp.Net MVC",
              "BASIC",
              "C",
              "C#",
              "C++",
              "Clojure",
              "COBOL",
              "ColdFusion",
              "Dojo",
              "Erlang",
              "Entity framework",
              "F#",
              "Fortran",
              "Groovy",
              "Haskell",
              "HTML",
              "HTML5",
              "iOS",
              "Java",
              "J2EE",
              "JavaScript",
              "Lisp",
              "LINQ",
              "LINUX",
              ".Net",
              "Perl",
              "PHP",
              "Python",
              "RESTful",
              "Ruby",
              "Scala",
              "Scheme",
              "Vb",
              "VB Script",
              "Vb.Net",
              "VC++",
              "SQL Server",
              "Sybase Central",
              "XHTML",
              "XML",
              "Xamarin",
              "Winforms",
              "Windows Service",
              "Web Service",
              "WCF",
              "WPF",
              "Windows",
              "UNIX",
              "DB2",
              "Oracle",
              "Sybase Central",
              "PEARL",
              "CGI",
              "DevOpps",
              "ADO.Net",
              "Razor",
              "CSS",
              "PMP",
              "MS AXAPTA",
              "SCRUM",
              "Joomla",
              "DotNet nuke",
              ".Net nuke",
              "PHP",
              "MySQL"
    ];
    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }

    $("#keywords")
    // don't navigate away from the field on tab when selecting an item
              .bind("keydown", function (event) {
                  if (event.keyCode === $.ui.keyCode.TAB &&
                      $(this).data("ui-autocomplete").menu.active) {
                      event.preventDefault();
                  }
              })
              .autocomplete({
                  minLength: 0,
                  source: function (request, response) {
                      // delegate back to autocomplete, but extract the last term
                      response($.ui.autocomplete.filter(
                        availableTags, extractLast(request.term)));
                  },
                  focus: function () {
                      // prevent value inserted on focus
                      return false;
                  },
                  select: function (event, ui) {
                      var terms = split(this.value);
                      // remove the current input
                      terms.pop();
                      // add the selected item
                      terms.push(ui.item.value);
                      // add placeholder to get the comma-and-space at the end
                      terms.push("");
                      this.value = terms.join(", ");
                      return false;
                  }
              });
});


$(function () {
    var cities = [
              "Ahmadabad",
              "Bangalore/Bengaluru",
              "Bhagalpur",
              "Chandigarh",
              "Delhi/NCR",
              "Gurgaon",
              "Hyderabad/Secunderabad",
              "Kolkata",
              "Mumbai",
              "Noida",
              "Pune",
              "Patna",
              "Indore",
              "Chennai",
              "Nagpur",
              "Kanpur",
              "Lucknow",
              "Bhopal",
              "Ranchi",
              "Guwahati",
              "Surat",
              "Vishakhapatnam",
              "Bhubaneswar",
              "Calicut",
              "Kochin",
              "Thiruvananthapuram",
              "Bilaspur",
              "Meerut",
              "Ujjain",
              "Nashik",
              "Jaipur",
              "Allahabad"
    ];

    function split(val) {
        return val.split(/,\s*/);
    }

    function extractLast(term) {
        return split(term).pop();
    }

    $("#city")
    // don't navigate away from the field on tab when selecting an item
              .bind("keydown", function (event) {
                  if (event.keyCode === $.ui.keyCode.TAB &&
                      $(this).data("ui-autocomplete").menu.active) {
                      event.preventDefault();
                  }
              })
              .autocomplete({
                  minLength: 0,
                  source:
                      function (request, response) {
                          // delegate back to autocomplete, but extract the last term
                          response($.ui.autocomplete.filter(
                            cities, extractLast(request.term)));
                      },
                  focus: function () {
                      // prevent value inserted on focus
                      return false;
                  },
                  select: function (event, ui) {
                      var terms = split(this.value);
                      // remove the current input
                      terms.pop();
                      // add the selected item
                      terms.push(ui.item.value);
                      // add placeholder to get the comma-and-space at the end
                      terms.push("");
                      this.value = terms.join(", ");
                      return false;
                  }
              });
});