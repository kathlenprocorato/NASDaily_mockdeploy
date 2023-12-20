export const calculateSemester = () => {
  const currentDate = new Date(); //Second Semester of 2023-2024
  const currentMonth = currentDate.getMonth() + 1;
  const dayOfMonth = currentDate.getDate();

  let initialSemester = "";
  if (currentMonth === 7) {
    initialSemester = "Summer"; //Assuming July is for the summer semester
  } else if (currentMonth === 6) {
    initialSemester = dayOfMonth >= 15 ? "Second" : "Summer"; //First half of June is for second semmester, second half is for summer
  } else if (currentMonth === 8) {
    initialSemester = dayOfMonth <= 15 ? "Summer" : "First"; //First half of August is for summer, second half is for first semester
  } else if (currentMonth >= 9 && currentMonth <= 12) {
    initialSemester = "First"; // Assuming September to December is the first semester
  } else if (currentMonth >= 1 && currentMonth <= 5) {
    initialSemester = "Second"; // Assuming January to May is the second semester
  }

  return initialSemester;
};

export const calculateSchoolYear = () => {
  const currentDate = new Date(); // Current date and time
  const currentYear = currentDate.getFullYear();
  const currentMonth = currentDate.getMonth() + 1;
  const currentDay = currentDate.getDate();

  let initialSchoolYear = "";

  if ((currentMonth === 6 && currentDay >= 16) || currentMonth > 6) {
    // From June 16 (this year) to June 15 (next year)
    initialSchoolYear = `${currentYear % 100}${(currentYear % 100) + 1}`.padStart(4, "20");
  } else {
    // Before June 16 (this year)
    initialSchoolYear = `${(currentYear % 100) - 1}${currentYear % 100}`.padStart(4, "20");
  }

  return initialSchoolYear;
};

export const getCurrentMonth = () => {
  const currentDate = new Date();
  const currentMonth = currentDate.toLocaleString("en-US", { month: "long" });

  return currentMonth;
};
