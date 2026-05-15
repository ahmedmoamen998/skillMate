function filterSkills() {
    let searchInput = document.getElementById("skillSearch");
    let categoryFilter = document.getElementById("categoryFilter");
    let levelFilter = document.getElementById("levelFilter");

    if (!searchInput || !categoryFilter || !levelFilter) {
        return;
    }

    let searchValue = searchInput.value.toLowerCase();
    let categoryValue = categoryFilter.value;
    let levelValue = levelFilter.value;

    let cards = document.querySelectorAll(".skill-card");

    cards.forEach(function (card) {
        let skill = card.getAttribute("data-skill");
        let instructor = card.getAttribute("data-instructor");
        let category = card.getAttribute("data-category");
        let level = card.getAttribute("data-level");

        let matchesSearch =
            skill.includes(searchValue) ||
            instructor.includes(searchValue);

        let matchesCategory =
            categoryValue === "" || category === categoryValue;

        let matchesLevel =
            levelValue === "" || level === levelValue;

        if (matchesSearch && matchesCategory && matchesLevel) {
            card.style.display = "block";
        } else {
            card.style.display = "none";
        }
    });
}

function validateSkillForm() {
    let skillName = document.getElementById("SkillName").value.trim();
    let category = document.getElementById("Category").value.trim();
    let instructorName = document.getElementById("InstructorName").value.trim();
    let level = document.getElementById("Level").value.trim();
    let availableTime = document.getElementById("AvailableTime").value.trim();
    let maxStudents = document.getElementById("MaxStudents").value;
    let contactInfo = document.getElementById("ContactInfo").value.trim();
    let description = document.getElementById("Description").value.trim();

    if (
        skillName === "" ||
        category === "" ||
        instructorName === "" ||
        level === "" ||
        availableTime === "" ||
        contactInfo === "" ||
        description === ""
    ) {
        alert("Please fill all required fields.");
        return false;
    }

    if (maxStudents <= 0) {
        alert("Max Students must be greater than 0.");
        return false;
    }

    return true;
}

function validateRequestForm() {
    let studentName = document.getElementById("StudentName").value.trim();
    let skillName = document.getElementById("SkillName").value.trim();
    let category = document.getElementById("Category").value.trim();
    let level = document.getElementById("Level").value.trim();
    let availableTime = document.getElementById("AvailableTime").value.trim();
    let maxStudents = document.getElementById("MaxStudents").value;
    let contactInfo = document.getElementById("ContactInfo").value.trim();
    let description = document.getElementById("Description").value.trim();

    if (
        studentName === "" ||
        skillName === "" ||
        category === "" ||
        level === "" ||
        availableTime === "" ||
        contactInfo === "" ||
        description === ""
    ) {
        alert("Please fill all required fields.");
        return false;
    }

    if (maxStudents <= 0) {
        alert("Max Students must be greater than 0.");
        return false;
    }

    return true;
}

function validateRegisterForm() {
    let fullName = document.getElementById("FullName").value.trim();
    let email = document.getElementById("Email").value.trim();
    let password = document.getElementById("Password").value;
    let confirmPassword = document.getElementById("ConfirmPassword").value;

    if (fullName === "" || email === "" || password === "" || confirmPassword === "") {
        alert("Please fill all required fields.");
        return false;
    }

    if (password !== confirmPassword) {
        alert("Password and Confirm Password do not match.");
        return false;
    }

    return true;
}