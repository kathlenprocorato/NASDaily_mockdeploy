import { useMemo, useRef, useEffect, useState } from "react";
import axios from "axios";

export const AddSuperiorForm = () => {
  const [offices, setOffices] = useState([]);
  const lastnameRef = useRef();
  const firstnameRef = useRef();
  const officeRef = useRef();

  const api = useMemo(
    () =>
      axios.create({
        baseURL: "https://localhost:7001/api",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }),
    []
  );

  useEffect(() => {
    const fetchOffices = async () => {
      try {
        const response = await api.get(`/Offices`);
        setOffices(response.data);
        console.log(response.data);
      } catch (error) {
        console.error(error);
      }
    };
    fetchOffices();
  }, [api]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    const lastname = lastnameRef.current.value;
    const firstname = firstnameRef.current.value;
    const officeId = officeRef.current.value;

    const username = `${firstname}${lastname}`.toLowerCase();
    console.log(username);
    console.log(officeId);

    // REGISTER AS USER
    //   try {
    //     const registeruser = await api.post(`/Auth/register/`, {
    //       username: username,
    //       password: username,
    //       role: "Superior",
    //     });
    //     console.log(registeruser);
    //   } catch (error) {
    //     console.error(error);
    //   }

    //   REGISTER AS SUPERIOR
    // try {
    //   const registersuperior = await api.post(`/Superiors`, {
    //     firstName: firstname,
    //     lastName: lastname,
    //     username: username,
    //   });
    //   console.log(registersuperior);
    // } catch (error) {
    //   console.error(error);
    // }
  };

  return (
    <div>
      <section>
        <div className="max-w-full mb-10">
          <h2 className="mb-4 text-xl font-bold text-black">Add a Superior</h2>
          <form action="#">
            <div className="grid grid-cols-3 gap-6">
              <div className="w-full">
                <label htmlFor="lastname" className="block mb-2 text-sm font-medium text-gray-900">
                  Last Name
                </label>
                <input
                  ref={lastnameRef}
                  type="text"
                  name="lastname"
                  id="lastname"
                  className="bg-gray-50 border border-gray text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 "
                  placeholder="Last name"
                  required=""
                />
              </div>
              <div className="w-full">
                <label htmlFor="firstname" className="block mb-2 text-sm font-medium text-gray-900">
                  First Name
                </label>
                <input
                  ref={firstnameRef}
                  type="text"
                  name="firstname"
                  id="firstname"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  placeholder="First name"
                  required=""
                />
              </div>
              <div>
                <label htmlFor="office" className="block mb-2 text-sm font-medium text-gray-900">
                  Assigned Office
                </label>
                <select
                  ref={officeRef}
                  id="category"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5"
                >
                  <option selected="">Select office</option>
                  {offices.map((office) => (
                    <option key={office.id} value={office.id}>
                      {office.officeName}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <button
              type="submit"
              className="inline-flex items-center px-5 py-2.5 mt-4 sm:mt-6 text-sm font-medium text-center text-black bg-secondary rounded-lg focus:ring-4 focus:ring-primary-200 hover:bg-primary hover:text-white"
              onClick={handleSubmit}
            >
              Add
            </button>
          </form>
        </div>
      </section>
    </div>
  );
};
