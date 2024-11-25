import { useState, useEffect } from "react";

const formatData = (dateString) => {
  const [formattedDate, setFormattedDate] = useState("");

  useEffect(() => {
    const formatDate = () => {
      const date = new Date(dateString);
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, "0");
      const day = String(date.getDate()).padStart(2, "0");
      return `${year}/${month}/${day}`;
    };

    if (dateString) {
      setFormattedDate(formatDate());
    }
  }, [dateString]);

  return formattedDate;
};

export default formatData;
