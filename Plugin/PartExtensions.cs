/* Copyright © 2013-2014, Elián Hanisch <lambdae2@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;

namespace RCSBuildAid
{
    public static class PartExtensions
    {
        static HashSet<int> nonPhysicsModules = new HashSet<int> {
            "ModuleLandingGear".GetHashCode(),
            "LaunchClamp".GetHashCode(), /* has mass at launch, but accounting it is worthless */
        };

        static HashSet<int> nonPhysicsParts = new HashSet<int> {
            "ladder1".GetHashCode(),
            "telescopicLadder".GetHashCode(),
            "telescopicLadderBay".GetHashCode(),
        };

        public static bool physicalSignificance (this Part part)
        {
            if (part.physicalSignificance == Part.PhysicalSignificance.FULL) {
                if (nonPhysicsParts.Contains (part.partInfo.name.GetHashCode())) {
                    return false;
                }
                IEnumerator<PartModule> enm = (IEnumerator<PartModule>)part.Modules.GetEnumerator ();
                while (enm.MoveNext()) {
                    PartModule mod = enm.Current;
                    if (nonPhysicsModules.Contains (mod.ClassID)) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static float GetResourceMassFixed (this Part part) {
            float mass = part.GetResourceMass();
            /* with some outdated mods, it can return NaN */
            if (float.IsNaN(mass)) {
                return 0;
            }
            return mass;
        }

        public static float GetTotalMass (this Part part) {
            return part.GetResourceMassFixed() + part.mass;
        }
    }
}

