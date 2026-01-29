export const currentDate = new Date();

export const nextYearDate = new Date();
nextYearDate.setFullYear(currentDate.getFullYear() + 1);

export const oneMonthAgoDate = new Date();
oneMonthAgoDate.setMonth(currentDate.getMonth() - 1);

export const nextMonthDate = new Date();
nextMonthDate.setMonth(currentDate.getMonth() + 1);
