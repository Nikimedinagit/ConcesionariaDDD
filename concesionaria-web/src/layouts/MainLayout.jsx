import { Outlet } from "react-router-dom";

import { NavbarHeader } from "@/components/layout/NavbarHeader";
import { Footer } from "@/components/layout/Footer";

export function MainLayout() {
  return (
    <div className="mb-3 flex flex-col">
      <NavbarHeader />

      <main className="flex-1 w-full pb-12">
        <div className="mx-auto w-full max-w-[1440px] px-2 md:px-4">
          <Outlet />
        </div>
      </main>

      <Footer />
    </div>
  );
}
