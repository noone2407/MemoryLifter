/***************************************************************************************************************************************
 * Copyright (C) 2001-2012 LearnLift USA																	*
 * Contact: Learnlift USA, 12 Greenway Plaza, Suite 1510, Houston, Texas 77046, support@memorylifter.com					*
 *																								*
 * This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License	*
 * as published by the Free Software Foundation; either version 2.1 of the License, or (at your option) any later version.			*
 *																								*
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty	*
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.	*
 *																								*
 * You should have received a copy of the GNU Lesser General Public License along with this library; if not,					*
 * write to the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA					*
 ***************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MLifter.DAL.Interfaces;
using MLifter.DAL.Tools;

namespace MLifter.DAL.XML
{
    internal class XmlAllowedQueryDirections : IQueryDirections
    {
        XmlElement m_QueryDirections;
        const string m_XPathGeneral = "/dictionary/general";
        const string m_XPathAllowedQueryDirections = "allowedquerydirections";
        const string m_XPathBasePath = m_XPathGeneral + "/" + m_XPathAllowedQueryDirections;
        const string m_XPathQueryDirection = "querydirection";
        const string m_XPathAttributeName = "name";
        const string m_XPathAttributeValue = "value";

        const string m_XPathFilter = "querydirection[@name='{0}']/@value";

        internal XmlAllowedQueryDirections(XmlDictionary dictionary, ParentClass parent)
        {
            m_QueryDirections = (XmlElement)dictionary.Dictionary.SelectSingleNode(m_XPathBasePath);
            if (m_QueryDirections == null)
            {
                m_QueryDirections = XmlHelper.CreateAndAppendElement((XmlElement)dictionary.Dictionary.SelectSingleNode(m_XPathGeneral), m_XPathAllowedQueryDirections);
                AppendMode(EQueryDirection.Question2Answer, true);
                AppendMode(EQueryDirection.Answer2Question, true);
                AppendMode(EQueryDirection.Mixed, true);
            }
            this.parent = parent;
        }

        #region IQueryDirection Members

        public bool? Question2Answer
        {
            get
            {
                return IsAllowed(EQueryDirection.Question2Answer);
            }
            set
            {
                SetAllowed(EQueryDirection.Question2Answer, value.GetValueOrDefault());
            }
        }

        public bool? Answer2Question
        {
            get
            {
                return IsAllowed(EQueryDirection.Answer2Question);
            }
            set
            {
                SetAllowed(EQueryDirection.Answer2Question, value.GetValueOrDefault());
            }
        }

        public bool? Mixed
        {
            get
            {
                return IsAllowed(EQueryDirection.Mixed);
            }
            set
            {
                SetAllowed(EQueryDirection.Mixed, value.GetValueOrDefault());
            }
        }

        #endregion

        private bool IsAllowed(EQueryDirection queryDir)
        {
            bool allowed = false;
            XmlNode node = m_QueryDirections.SelectSingleNode(String.Format(m_XPathFilter, queryDir.ToString()));
            if (node == null)
                allowed = true;
            else
                allowed = XmlConvert.ToBoolean(node.Value);
            return allowed;
        }

        private void SetAllowed(EQueryDirection queryDir, bool allowed)
        {
            XmlNode node = m_QueryDirections.SelectSingleNode(String.Format(m_XPathFilter, queryDir.ToString()));
            if (node == null)
            {
                AppendMode(queryDir, allowed);
            }
            else
            {
                node.Value = XmlConvert.ToString(allowed);
            }
        }

        private XmlElement AppendMode(EQueryDirection queryDir, bool allowed)
        {
            XmlElement eWord = XmlHelper.CreateAndAppendElement(m_QueryDirections, m_XPathQueryDirection);
            eWord.SetAttribute(m_XPathAttributeName, queryDir.ToString());
            eWord.SetAttribute(m_XPathAttributeValue, XmlConvert.ToString(allowed));
            return eWord;
        }

        #region ICopy Members

        public void CopyTo(MLifter.DAL.Tools.ICopy target, CopyToProgress progressDelegate)
        {
            CopyBase.Copy(this, target, typeof(IQueryDirections), progressDelegate);
        }

        #endregion

        #region IParent Members

        private ParentClass parent;

        public ParentClass Parent
        {
            get { return parent; }
        }

        #endregion
    }
}
