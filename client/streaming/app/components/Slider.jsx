"use client";
import { useRouter } from "next/navigation";

const Slider = ({ items = [] }) => {
  const router = typeof window !== "undefined" ? useRouter() : null;

  const handleClick = (id) => {
    if (router) {
      router.push(`/movie/${id}`);
    }
  };

  // console.log(items);

  return (
    <div className="overflow-hidden">
      <div className="flex gap-4 p-4 overflow-x-scroll no-scrollbar">
        {items.map((item) => (
          <div
            key={item.id}
            className="w-64 h-80 flex-shrink-0 rounded-lg shadow-lg overflow-hidden cursor-pointer"
            onClick={() => handleClick(item.id)}
          >
            <div className="w-full h-full relative">
              <img
                src={item.picture}
                alt={item.name}
                className="w-full h-full object-cover"
              />
              <div className="absolute inset-0 bg-black bg-opacity-50 p-4 flex flex-col justify-end">
                <h3 className="text-white font-semibold text-lg">
                  {item.name}
                </h3>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Slider;
